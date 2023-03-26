import React from "react";
import { Provider } from "mobx-react";
import exchangeRateStore from "./stores/ExchangeRateStore";
import App from "./App";

function Root() {
  return (
    <Provider exchangeRateStore={exchangeRateStore}>
      <App />
    </Provider>
  );
}

export default Root;
