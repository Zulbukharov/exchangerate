import React from "react";

const PageHeader = ({ title }) => {
  return (
    <header className="bg-indigo-600 py-6">
      <div className="container mx-auto px-6">
        <h1 className="text-3xl font-bold text-white">{title}</h1>
      </div>
    </header>
  );
};

export default PageHeader;
