import { useState, useEffect } from 'react';

function ExchangeRate() {
  const [rates, setRates] = useState([]);

  useEffect(() => {
    const fetchRates = async () => {
      const response = await fetch('/exchangerates');
      const data = await response.json();
      setRates(data);
    };

    fetchRates();
  }, []);

  return (
    <div>
      <h2>Exchange Rates</h2>
      <table>
        <thead>
          <tr>
            <th>Currency</th>
            <th>Rate</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          {rates.map((rate) => (
            <tr key={rate.id}>
              <td>{rate.currency}</td>
              <td>{rate.rate}</td>
              <td>{new Date(rate.date).toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ExchangeRate;
