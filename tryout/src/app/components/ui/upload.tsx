import React from "react";

interface ImageUploadProps {
  label?: string;
  name: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const ImageUpload: React.FC<ImageUploadProps> = ({ label, name, onChange }) => {
  return (
    <div className="flex flex-col gap-1">
      {label && (
        <label htmlFor={name} className="text-sm font-bold text-gray-700">
          {label}
        </label>
      )}

      <input
        type="file"
        accept="image/*"
        id={name}
        name={name}
        onChange={onChange}
        className="w-full border border-gray-300 rounded-md p-2 bg-gray-300/50 hover:bg-gray-500/70 cursor-pointer"
      />
    </div>
  );
};

export default ImageUpload;
