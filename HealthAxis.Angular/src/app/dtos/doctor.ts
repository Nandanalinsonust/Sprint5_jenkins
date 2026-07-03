import { Specialisation } from '../enums/specialisation';

export interface Doctor {

  doctorId: number;

  fullName: string;

  email: string;

  specialisation: Specialisation;

  yearsOfExperience: number;

  consultationFee: number;

  isActive: boolean;
}

export interface UpdateDoctorRequest {

  doctorId: number;

  fullName: string;

  specialisation: Specialisation;

  yearsOfExperience: number;

  consultationFee: number;

  isActive: boolean;
}