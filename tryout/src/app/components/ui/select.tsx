import React from "react";

interface SelectProps {
  label: string;
  name: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  options: { label: string; value: string }[];
  error?: string; 
}

const Select: React.FC<SelectProps> = ({
  label,
  name,
  value,
  onChange,
  options,
  error,
}) => {
  return (
    <div>
      <label
        htmlFor={name}
        className="block text-sm font-medium text-stone-700"
      >
        {label}
      </label>
      <div className="mt-1">
        <select
          id={name}
          name={name}
          value={value}
          onChange={onChange}
          className="block w-full px-3 py-2 border border-gray-400 rounded-md"
        >
          {options.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
      </div>
      {error && <p className="text-red-500 text-sm mt-1">{error}</p>}
    </div>
  );
};

export default Select;
