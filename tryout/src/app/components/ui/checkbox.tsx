import React from "react";

interface CheckboxProps {
  label: string;
  name: string;
  checked: boolean;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const Checkbox: React.FC<CheckboxProps> = ({
  label,
  name,
  checked,
  onChange,
}) => {
  return (
    <div className="flex items-center">
      <input
        id={name}
        name={name}
        type="checkbox"
        checked={checked}
        onChange={onChange}
        className="w-4 h-4 accent-black rounded"
      />
      <label
        htmlFor={name}
        className="block ml-3 text-sm font-medium text-gray-700"
      >
        {label}
      </label>
    </div>
  );
};

export default Checkbox;
