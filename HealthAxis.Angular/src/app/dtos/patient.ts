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

export interface CreatePatientProfileRequest {

  fullName: string;

  dateOfBirth: string;

  gender: string;

  email: string;

  phoneNumber: string;

  insuranceID?: string;
}

export interface UpdatePatientProfileRequest {

  patientId: number;

  fullName: string;

  dateOfBirth: string;

  gender: string;

  phoneNumber: string;

  insuranceID?: string;

  isActive: boolean;
}