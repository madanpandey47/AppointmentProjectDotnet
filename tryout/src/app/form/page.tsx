"use client";
import React, { useState, ChangeEvent } from "react";
import Form from "../components/ui/form";
import Input from "../components/ui/input";
import Radio from "../components/ui/radio";
import Select from "../components/ui/select";
import Checkbox from "../components/ui/checkbox";
import ImageUpload from "../components/ui/upload";
import Button from "../components/ui/button";
import { AiOutlineEye, AiOutlineEyeInvisible } from "react-icons/ai";

// Type for the form state
type FormType = {
  firstname: string;
  lastname: string;
  email: string;
  alternateEmail: string;
  description: string;
  gender: string;
  bloodgroup: string;
  primaryMobile: string;
  alternateMobile: string;
  emergencyContact: string;
  emergencyRelation: string;
  maritalStatus: string;
  religion: string;
  ethnicity: string;
  province: string;
  district: string;
  municipality: string;
  wardNo: string;
  altProvince: string;
  altDistrict: string;
  altMunicipality: string;
  altWardNo: string;
  country: string;
  password: string;
  confirmPassword: string;
  agree: boolean;
  fatherName: string;
  fatherOccupation: string;
  fatherDesignation: string;
  fatherOrganization: string;
  fatherMobile: string;
  fatherEmail: string;
  motherName: string;
  motherOccupation: string;
  motherDesignation: string;
  motherOrganization: string;
  motherMobile: string;
  motherEmail: string;
  guardianName: string;
  guardianRelation: string;
  guardianOccupation: string;
  guardianMobile: string;
  guardianEmail: string;
  annualIncome: string;
  primaryContact: "father" | "mother" | "guardian";
};

const FormPage: React.FC = () => {
  const [currentStep, setCurrentStep] = useState(1);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [file, setFile] = useState<File | null>(null);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  const [form, setForm] = useState<FormType>({
    firstname: "",
    lastname: "",
    email: "",
    alternateEmail: "",
    description: "",
    gender: "",
    bloodgroup: "",
    primaryMobile: "",
    alternateMobile: "",
    emergencyContact: "",
    emergencyRelation: "",
    maritalStatus: "",
    religion: "",
    ethnicity: "",
    province: "",
    district: "",
    municipality: "",
    wardNo: "",
    altProvince: "",
    altDistrict: "",
    altMunicipality: "",
    altWardNo: "",
    country: "nepal",
    password: "",
    confirmPassword: "",
    agree: false,
    fatherName: "",
    fatherOccupation: "",
    fatherDesignation: "",
    fatherOrganization: "",
    fatherMobile: "",
    fatherEmail: "",
    motherName: "",
    motherOccupation: "",
    motherDesignation: "",
    motherOrganization: "",
    motherMobile: "",
    motherEmail: "",
    guardianName: "",
    guardianRelation: "",
    guardianOccupation: "",
    guardianMobile: "",
    guardianEmail: "",
    annualIncome: "",
    primaryContact: "father",
  });

  const bloodGroups = [
    { label: "A+", value: "A+" },
    { label: "A-", value: "A-" },
    { label: "B+", value: "B+" },
    { label: "B-", value: "B-" },
    { label: "AB+", value: "AB+" },
    { label: "O+", value: "O+" },
    { label: "O-", value: "O-" },
  ];

  const incomeOptions = [
    { label: "<5 Lakh", value: "<5" },
    { label: "5-10 Lakh", value: "5-10" },
    { label: "10-20 Lakh", value: "10-20" },
    { label: ">20 Lakh", value: ">20" },
  ];

  const handleChange = (
    e: ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
  ) => {
    const { name, value, type } = e.target;
    let val: string | boolean = value;
    if (type === "checkbox") {
      val = (e.target as HTMLInputElement).checked;
    }
    setForm((prevForm) => ({ ...prevForm, [name]: val }));

    // Clear error for this field when user types
    if (errors[name]) {
      setErrors((prev) => {
        const newErrs = { ...prev };
        delete newErrs[name];
        return newErrs;
      });
    }
  };

  // --- VALIDATION LOGIC PER STEP ---
  const validateStep = (step: number) => {
    const newErrors: { [key: string]: string } = {};
    let isValid = true;

    // STEP 1: Personal Info
    if (step === 1) {
      if (!form.firstname) newErrors.firstname = "First name is required";
      if (!form.lastname) newErrors.lastname = "Last name is required";
      if (!form.email) newErrors.email = "Email is required";
      if (!form.gender) newErrors.gender = "Gender is required";
      if (!form.bloodgroup) newErrors.bloodgroup = "Blood group is required";
      if (!form.country) newErrors.country = "Country is required";
    }

    // STEP 2: Contact Details
    if (step === 2) {
      if (!form.primaryMobile)
        newErrors.primaryMobile = "Primary mobile is required";
      if (!form.emergencyContact)
        newErrors.emergencyContact = "Emergency contact is required";
      if (!form.emergencyRelation)
        newErrors.emergencyRelation = "Relation is required";
    }

    // STEP 3: Family Details
    if (step === 3) {
      if (!form.annualIncome)
        newErrors.annualIncome = "Annual income is required";

      // Validate Father/Mother regardless (as per your original logic) or based on requirements
      if (!form.fatherName) newErrors.fatherName = "Father's name is required";
      if (!form.fatherMobile)
        newErrors.fatherMobile = "Father's mobile is required";
      if (!form.motherName) newErrors.motherName = "Mother's name is required";
      if (!form.motherMobile)
        newErrors.motherMobile = "Mother's mobile is required";

      if (form.primaryContact === "guardian") {
        if (!form.guardianName)
          newErrors.guardianName = "Guardian name is required";
        if (!form.guardianRelation)
          newErrors.guardianRelation = "Guardian relation is required";
      }
    }

    // STEP 4: Account Setup (Final validation)
    if (step === 4) {
      if (!file) newErrors.file = "Please upload profile image";
      if (!form.password) newErrors.password = "Password is required";
      if (form.password !== form.confirmPassword) {
        newErrors.confirmPassword = "Passwords do not match";
      }
      if (!form.agree) newErrors.agree = "You must agree to terms";
    }

    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      isValid = false;
    } else {
      setErrors({});
    }

    return isValid;
  };

  const handleNext = () => {
    if (validateStep(currentStep)) {
      setCurrentStep((prev) => prev + 1);
    }
  };

  const handleBack = () => {
    setCurrentStep((prev) => prev - 1);
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    // Validate final step
    if (validateStep(4)) {
      console.log("Submitted Data:", { ...form, file });
      alert("Form Submitted Successfully!");
    }
  };

  return (
    <div className="flex items-center justify-center min-h-screen bg-slate-300 p-4">
      <Form onSubmit={handleSubmit}>
        <h1 className="text-2xl font-bold text-center mb-4">
          Application Form
        </h1>

        {/* Progress Indicator */}
        <div className="mb-6 text-center text-sm font-semibold text-gray-500">
          Step {currentStep} of 4
        </div>

        {/* --- STEP 1: PERSONAL INFO --- */}
        {currentStep === 1 && (
          <div className="space-y-4">
            <h2 className="text-xl font-semibold border-b pb-2">
              Personal Information
            </h2>
            <div className="grid grid-cols-2 gap-4">
              <Input
                label="First Name"
                name="firstname"
                value={form.firstname}
                onChange={handleChange}
                error={errors.firstname}
              />
              <Input
                label="Last Name"
                name="lastname"
                value={form.lastname}
                onChange={handleChange}
                error={errors.lastname}
              />
            </div>
            <Input
              label="Email"
              name="email"
              type="email"
              value={form.email}
              onChange={handleChange}
              error={errors.email}
            />
            <Input
              label="Alternate Email"
              name="alternateEmail"
              value={form.alternateEmail}
              onChange={handleChange}
              error={errors.alternateEmail}
            />
            <Select
              label="Country"
              name="country"
              value={form.country}
              onChange={handleChange}
              options={[
                { label: "Nepal", value: "nepal" },
                { label: "India", value: "india" },
                { label: "China", value: "china" },
              ]}
              error={errors.country}
            />
            <Select
              label="Blood Group"
              name="bloodgroup"
              value={form.bloodgroup}
              onChange={handleChange}
              options={bloodGroups}
              error={errors.bloodgroup}
            />
            <Radio
              label="Gender"
              name="gender"
              selectedValue={form.gender}
              onChange={(e) => handleChange(e)}
              options={[
                { label: "Male", value: "male" },
                { label: "Female", value: "female" },
                { label: "Other", value: "other" },
              ]}
              error={errors.gender}
            />
          </div>
        )}

        {/* --- STEP 2: CONTACT INFO --- */}
        {currentStep === 2 && (
          <div className="space-y-4">
            <h2 className="text-xl font-semibold border-b pb-2">
              Contact Details
            </h2>
            <div className="grid grid-cols-2 gap-4">
              <Input
                label="Primary Mobile"
                name="primaryMobile"
                value={form.primaryMobile}
                onChange={handleChange}
                error={errors.primaryMobile}
              />
              <Input
                label="Alternate Mobile"
                name="alternateMobile"
                value={form.alternateMobile}
                onChange={handleChange}
              />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <Input
                label="Emergency Contact"
                name="emergencyContact"
                value={form.emergencyContact}
                onChange={handleChange}
                error={errors.emergencyContact}
              />
              <Input
                label="Emergency Relation"
                name="emergencyRelation"
                value={form.emergencyRelation}
                onChange={handleChange}
                error={errors.emergencyRelation}
              />
            </div>
          </div>
        )}

        {/* --- STEP 3: FAMILY DETAILS --- */}
        {currentStep === 3 && (
          <div className="space-y-6">
            <h2 className="text-xl font-semibold border-b pb-2">
              Family Details
            </h2>
            <Radio
              label="Primary Contact"
              name="primaryContact"
              selectedValue={form.primaryContact}
              onChange={(e) => handleChange(e)}
              options={[
                { label: "Father", value: "father" },
                { label: "Mother", value: "mother" },
                { label: "Guardian", value: "guardian" },
              ]}
            />

            <div className="bg-slate-50 p-4 rounded-md">
              <h3 className="font-bold mb-2">Father&apos;s Details</h3>
              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Name"
                  name="fatherName"
                  value={form.fatherName}
                  onChange={handleChange}
                  error={errors.fatherName}
                />
                <Input
                  label="Mobile"
                  name="fatherMobile"
                  value={form.fatherMobile}
                  onChange={handleChange}
                  error={errors.fatherMobile}
                />
                <Input
                  label="Occupation"
                  name="fatherOccupation"
                  value={form.fatherOccupation}
                  onChange={handleChange}
                />
                <Input
                  label="Email"
                  name="fatherEmail"
                  value={form.fatherEmail}
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="bg-slate-50 p-4 rounded-md">
              <h3 className="font-bold mb-2">Mother&apos;s Details</h3>
              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Name"
                  name="motherName"
                  value={form.motherName}
                  onChange={handleChange}
                  error={errors.motherName}
                />
                <Input
                  label="Mobile"
                  name="motherMobile"
                  value={form.motherMobile}
                  onChange={handleChange}
                  error={errors.motherMobile}
                />
                <Input
                  label="Occupation"
                  name="motherOccupation"
                  value={form.motherOccupation}
                  onChange={handleChange}
                />
                <Input
                  label="Email"
                  name="motherEmail"
                  value={form.motherEmail}
                  onChange={handleChange}
                />
              </div>
            </div>

            {form.primaryContact === "guardian" && (
              <div className="bg-slate-50 p-4 rounded-md border-l-4 border-blue-500">
                <h3 className="font-bold mb-2">Guardian&apos;s Details</h3>
                <div className="grid grid-cols-2 gap-4">
                  <Input
                    label="Name"
                    name="guardianName"
                    value={form.guardianName}
                    onChange={handleChange}
                    error={errors.guardianName}
                  />
                  <Input
                    label="Relation"
                    name="guardianRelation"
                    value={form.guardianRelation}
                    onChange={handleChange}
                    error={errors.guardianRelation}
                  />
                  <Input
                    label="Mobile"
                    name="guardianMobile"
                    value={form.guardianMobile}
                    onChange={handleChange}
                    error={errors.guardianMobile}
                  />
                  <Input
                    label="Email"
                    name="guardianEmail"
                    value={form.guardianEmail}
                    onChange={handleChange}
                  />
                </div>
              </div>
            )}

            <Select
              label="Annual Family Income"
              name="annualIncome"
              value={form.annualIncome}
              onChange={handleChange}
              options={incomeOptions}
              error={errors.annualIncome}
            />
          </div>
        )}

        {/* --- STEP 4: ACCOUNT & REVIEW --- */}
        {currentStep === 4 && (
          <div className="space-y-4">
            <h2 className="text-xl font-semibold border-b pb-2">
              Account Setup
            </h2>

            <ImageUpload
              label="Upload Profile Image"
              name="profile"
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setFile(e.target.files ? e.target.files[0] : null)
              }
            />
            {errors.file && (
              <p className="text-red-500 text-sm">{errors.file}</p>
            )}

            <div className="relative">
              <Input
                label="Password"
                name="password"
                type={showPassword ? "text" : "password"}
                value={form.password}
                onChange={handleChange}
                error={errors.password}
              />
              <button
                type="button"
                onClick={() => setShowPassword(!showPassword)}
                className="absolute top-10 right-3"
              >
                {showPassword ? (
                  <AiOutlineEyeInvisible size={20} />
                ) : (
                  <AiOutlineEye size={20} />
                )}
              </button>
            </div>

            <div className="relative">
              <Input
                label="Confirm Password"
                name="confirmPassword"
                type={showConfirmPassword ? "text" : "password"}
                value={form.confirmPassword}
                onChange={handleChange}
                error={errors.confirmPassword}
              />
              <button
                type="button"
                onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                className="absolute top-10 right-3"
              >
                {showConfirmPassword ? (
                  <AiOutlineEyeInvisible size={20} />
                ) : (
                  <AiOutlineEye size={20} />
                )}
              </button>
            </div>

            <Checkbox
              label="I agree to the terms and conditions"
              name="agree"
              checked={form.agree}
              onChange={handleChange}
            />
            {errors.agree && (
              <p className="text-red-500 text-sm">{errors.agree}</p>
            )}
          </div>
        )}

        {/* --- BUTTONS --- */}
        <div className="mt-8 flex justify-between gap-4">
          {currentStep > 1 && (
            <button
              type="button"
              onClick={handleBack}
              className="px-4 py-2 bg-gray-500 text-white rounded hover:bg-gray-600"
            >
              Back
            </button>
          )}

          {currentStep < 4 ? (
            <button
              type="button"
              onClick={handleNext}
              className="ml-auto px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
            >
              Next
            </button>
          ) : (
            <Button type="submit" label="Submit Application" />
          )}
        </div>
      </Form>
    </div>
  );
};

export default FormPage;
