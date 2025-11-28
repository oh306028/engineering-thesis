import axios from "axios";

export default class AccountService {
  public static async Login(formData: LoginForm): Promise<string> {
    const response = await axios.post("/account/login", formData);
    return response.data.token;
  }

  public static async Register(formData: RegisterForm): Promise<LoginForm> {
    const response = await axios.post("/account/register", formData);
    return response.data;
  }
}

export interface LoginForm {
  login: string;
  password: string;
}

export interface ValidationError {
  [key: string]: string[];
}

export interface RegisterForm {
  parentFirstName: string;
  parentLastName: string;
  email: string;
  login: string;
  password: string;
  confirmPassword: string;

  studentFirstName: string;
  studentLastName: string;
  studentDateOfBirth?: Date;
}
