import "./App.css";
import ExchangeRate from "./ExchangeRate";
import PageHeader from "./Header";

function App() {
  return (
    <div className="App">
      <PageHeader
        title={`Exchange Rates for ${new Date().toLocaleDateString()}`}
      />
      <ExchangeRate />
    </div>
  );
}

export default App;
