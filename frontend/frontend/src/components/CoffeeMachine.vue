<template>
  <div class="full-height-container">
    <v-container fluid>
      <h1 class="text-center mb-2">CAFFÈ NERO</h1>
      <v-row class="content-row">
        <!-- Sección Izquierda: Lista de Cafés -->
        <v-col cols="6" class="coffee-list">
          <v-row>
            <v-col cols="12" v-for="(coffee, index) in coffees" :key="index" class="mb-2">
              <v-card outlined class="coffee-card">
                <v-card-text>
                  <div>
                    <h5 class="m-0">{{ coffee.name }}</h5>
                    <p class="m-0">₡{{ coffee.price }}</p>
                    <p>Disponibles: {{ coffee.stock }}</p>
                  </div>
                  <v-row class="mt-2 align-center">
                    <v-col cols="8">
                      <v-text-field
                        v-model="coffee.quantity"
                        type="number"
                        dense
                        outlined
                        placeholder="Cantidad"
                        min="0"
                        @input="onQuantityChange(coffee)"
                        class="quantity-input"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="4" class="d-flex align-center justify-end">
                      <v-btn @click="addToCart(coffee)" class="add-button" small block>
                        Agregar
                      </v-btn>
                    </v-col>
                  </v-row>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-col>

        <!-- Sección Derecha: Orden y Pago -->
        <v-col cols="6" class="summary-section">
          <v-card outlined class="order-summary-card">
            <v-card-title class="justify-center">
              <h4>Orden</h4>
            </v-card-title>
            <v-divider></v-divider>
            <v-card-text>
              <ul>
                <li v-for="(item, index) in cart" :key="index">
                  {{ item.name }}: {{ item.quantity }} unidades
                </li>
              </ul>
              <p><strong>Precio final:</strong> ₡{{ totalPrice }}</p>
              <p><strong>Monto seleccionado actual:</strong> ₡{{ currentAmount }}</p>
              <!-- Ingreso de billetes de 1000 -->
              <div class="mt-4">
                <p><strong>Billetes de ₡1000:</strong></p>
                <v-text-field
                  v-model="bill1000"
                  type="number"
                  dense
                  outlined
                  min="0"
                  placeholder="Ingresa la cantidad"
                  @input="updateCurrentAmount"
                  class="bill-input"
                ></v-text-field>
              </div>
              <!-- Ingreso de monedas -->
              <v-row class="mt-4">
                <v-col v-for="(coin, index) in coins" :key="index" cols="6" class="mb-2">
                  <div class="coin-container">
                    <span><strong>₡{{ coin.value }}</strong></span>
                    <v-text-field
                      v-model="coin.quantity"
                      type="number"
                      dense
                      outlined
                      min="0"
                      placeholder="Ingresa la cantidad"
                      @input="updateCurrentAmount"
                      class="coin-input"
                    ></v-text-field>
                  </div>
                </v-col>
              </v-row>
            </v-card-text>
            <v-card-actions class="justify-center">
              <v-btn @click="processPayment" class="pay-button" small>
                Pagar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>

      <!-- Dialogo para mostrar mensajes -->
      <v-dialog v-model="dialog" max-width="500">
        <v-card>
          <v-card-title class="headline">{{ dialogTitle }}</v-card-title>
          <v-card-text>{{ dialogMessage }}</v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="primary" text @click="dialog = false">Cerrar</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-container>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      coffees: [],
      coins: [],
      cart: [],
      totalPrice: 0,
      bill1000: 0,
      dialog: false,
      dialogTitle: "",
      dialogMessage: "",
      currentAmount: 0,
    };
  },
  async created() {
    try {
      const coffeeResponse = await axios.get("https://localhost:7015/api/Coffee");
      if (coffeeResponse.data.success && coffeeResponse.data.coffees) {
        this.coffees = coffeeResponse.data.coffees.map((coffee) => ({
          name: coffee.coffeeName,
          price: coffee.coffeePrice,
          stock: coffee.coffeeStock,
          quantity: 0,
        }));
      }

      const coinResponse = await axios.get("https://localhost:7015/api/Coin/all");
      if (coinResponse.data.success && coinResponse.data.coins) {
        this.coins = coinResponse.data.coins.map((coin) => ({
          value: coin.coinValue,
          stock: coin.coinStock,
          quantity: 0,
        }));
      }
    } catch (error) {
      this.showDialog("Error", "Error al cargar los datos. Por favor, inténtelo de nuevo más tarde.");
      console.error("Error al cargar los datos:", error);
    }
  },
  methods: {
    addToCart(coffee) {
      if (coffee.quantity > 0 && coffee.quantity <= coffee.stock) {
        const item = { ...coffee, quantity: coffee.quantity };
        this.cart.push(item);
        this.totalPrice += coffee.quantity * coffee.price;
        coffee.stock -= coffee.quantity;
        coffee.quantity = 0;
      } else {
        this.showDialog("Cantidad inválida", "La cantidad ingresada es inválida. Asegúrese de que la cantidad sea mayor a cero y no exceda el stock disponible.");
      }
    },
    onQuantityChange(coffee) {
      if (coffee.quantity < 0) {
        coffee.quantity = 0;
      }
    },
    onCoinQuantityChange(coin) {
      if (coin.quantity < 0) {
        coin.quantity = 0;
      }
    },
    updateCurrentAmount() {
      const billPayment = this.bill1000 * 1000;
      const coinPayment = this.coins.reduce((acc, coin) => acc + coin.value * coin.quantity, 0);
      this.currentAmount = billPayment + coinPayment;
    },
    async processPayment() {
      this.updateCurrentAmount();
      if (this.currentAmount < this.totalPrice) {
        this.showDialog("Monto insuficiente", "El monto ingresado es insuficiente para completar la compra.");
        return;
      }

      const changeToGive = this.currentAmount - this.totalPrice;

      try {
        if (changeToGive > 0) {
          const response = await axios.post("https://localhost:7015/api/Coin/calculate-exchange", {
            ChangeToGive: changeToGive,
          });

          if (response.data.success) {
            const exchangeDetails = response.data.exchange
              .map((coin) => `₡${coin.coinValue}: ${coin.coinStock} monedas`)
              .join("\n");
            this.showDialog("Pago exitoso", `Pago realizado correctamente. Cambio a entregar:\n${exchangeDetails}`);
          } else {
            this.showDialog("Cambio insuficiente", "No se puede dar el cambio exacto con las monedas disponibles.");
            return;
          }
        } else {
          this.showDialog("Pago exitoso", "Pago realizado correctamente sin necesidad de dar cambio.");
        }

        this.resetOrder();
      } catch (error) {
        this.showDialog("Error al procesar el pago", "Ocurrió un error al procesar el pago. Por favor, inténtelo de nuevo.");
        console.error("Error al procesar el pago:", error);
      }
    },
    resetOrder() {
      this.cart = [];
      this.totalPrice = 0;
      this.coins.forEach((coin) => (coin.quantity = 0));
      this.bill1000 = 0;
      this.coffees.forEach((coffee) => (coffee.quantity = 0));
      this.currentAmount = 0;
    },
    showDialog(title, message) {
      this.dialogTitle = title;
      this.dialogMessage = message;
      this.dialog = true;
    },
  },
};
</script>

<style scoped>
.full-height-container {
  height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.content-row {
  height: calc(100% - 50px);
}

.coffee-card {
  background-color: #d9d9d9;
  color: #333;
  padding: 10px;
}

.quantity-input {
  height: 30px;
  background-color: #f3f3f3;
  border-radius: 4px;
}

.coin-input,
.bill-input {
  height: 30px;
  background-color: #f3f3f3;
  border-radius: 4px;
}

.coin-container {
  display: flex;
  align-items: center;
  gap: 10px;
}

.add-button {
  background-color: #797777;
  color: white;
  text-transform: none;
}

.order-summary-card {
  background-color: #d9d9d9;
  color: black;
  border-radius: 4px;
  padding: 10px;
  height: calc(100% - 20px);
}

.summary-section {
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.pay-button {
  background-color: #797777;
  color: white;
  text-transform: none;
}

h1 {
  font-size: 1.5rem;
  margin-bottom: 5px;
}
</style>
