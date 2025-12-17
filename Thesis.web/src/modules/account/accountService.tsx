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

  public static async Verify(): Promise<void> {
    await axios.get("/account/verify");
  }

  public static async ImpersonateAsStudent(): Promise<string> {
    const response = await axios.get("account/impersonate-as-student");
    return response.data.token;
  }

  public static async MyProfile(): Promise<ProfileDetails> {
    const response = await axios.get("account/my-profile");
    return response.data;
  }

  public static async GetUserRole(): Promise<UserRoleModel> {
    const response = await axios.get("/account/role");
    return response.data;
  }

  public static async RegisterTeacher(formData: FormData): Promise<LoginForm> {
    const response = await axios.post("/account/register-teacher", formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
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

export interface UserRoleModel {
  role: string;
}

export interface ProfileDetails {
  fullName: string;
  email: string;
  login: string;
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

export interface TeacherAccountRegisterModel {
  email: string;
  password: string;
  confirmPassword: string;
  login: string;
  firstName: string;
  lastName: string;
  file: File;
}
