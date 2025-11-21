import React from "react";

interface RadioProps {
  label: string;
  name: string;
  options: { label: string; value: string }[];
  selectedValue: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  error?: string;
}

const Radio: React.FC<RadioProps> = ({
  label,
  name,
  options,
  selectedValue,
  onChange,
  error,
}) => {
  return (
    <fieldset>
      <legend className="text-base font-medium text-gray-900">{label}</legend>
      <div className="mt-4 flex gap-6">
        {options.map((option) => (
          <div key={option.value} className="flex items-center">
            <input
              id={`${name}-${option.value}`}
              name={name}
              type="radio"
              value={option.value}
              checked={selectedValue === option.value}
              onChange={onChange}
              className="w-4 h-4 accent-black rounded-full"
            />
            <label
              htmlFor={`${name}-${option.value}`}
              className="block ml-3 text-sm font-medium text-gray-700"
            >
              {option.label}
            </label>
          </div>
        ))}
      </div>
      {error && <p className="text-red-500 text-sm mt-1">{error}</p>}
    </fieldset>
  );
};

export default Radio;
