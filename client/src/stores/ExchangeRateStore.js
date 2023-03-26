import { makeAutoObservable } from "mobx";

class ExchangeRateStore {
  exchangeRates = [];

  constructor() {
    makeAutoObservable(this);
  }

  setExchangeRates(rates) {
    this.exchangeRates = rates;
  }
}

export default new ExchangeRateStore();
