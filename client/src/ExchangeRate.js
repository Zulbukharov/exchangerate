import { useState, useEffect } from "react";

function ExchangeRate() {
  const [rates, setRates] = useState([]);

  useEffect(() => {
    const fetchRates = async () => {
      const response = await fetch("/exchangerates");
      const data = await response.json();
      setRates(data);
    };

    fetchRates();
  }, []);

  return (
    <div className="max-w-md mx-auto bg-white rounded-xl shadow-md overflow-hidden md:max-w-2xl">
      <div className="overflow-y-auto h-screen">
        <ul className="divide-y divide-gray-200">
          {rates.map((rate) => (
            <li key={rate.currency} className="py-4  text-left">
              <div className="ml-3">
                <p className="m-0 text-bg font-medium text-gray-900">
                  {rate.currency}
                </p>
                <p className="text-sm text-gray-500">{`1 USD = ${rate.rate} ${rate.currency}`}</p>
              </div>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}

export default ExchangeRate;
