export interface PatientProfile {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceID?: string;
  isActive: boolean;
}

export interface PatientRegisterRequest {
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceID?: string;
}
