import axios from "axios";

export default class AdminService {
  public static async GetLogginsForStudent(): Promise<LogginHistoryModel[]> {
    const response = await axios.get("/admin/loggins/student");
    return response.data;
  }

  public static async GetTeacherAttempts(): Promise<
    TeacherAttemptsListModel[]
  > {
    const response = await axios.get("/admin/teacher-attempts");
    return response.data;
  }

  public static async GetAllUsers(
    filter: UsersFilter,
    pagination: PaginationEntry
  ): Promise<PaginationResult<UserListModel>> {
    const queryParams = {
      ...filter,
      ...pagination,
    };
    const response = await axios.get("/admin/users", {
      params: queryParams,
    });
    return response.data;
  }

  public static async GetAllClasses(
    filter: ClassroomsFilter,
    pagination: PaginationEntry
  ): Promise<PaginationResult<ClassroomListModel>> {
    const queryParams = {
      ...filter,
      ...pagination,
    };

    const response = await axios.get("/admin/classes", {
      params: queryParams,
    });
    return response.data;
  }

  public static async GetAllLoggins(
    filter: LogginsFilter,
    pagination: PaginationEntry
  ): Promise<PaginationResult<LogginHistoryListModel>> {
    const queryParams = {
      ...filter,
      ...pagination,
    };

    const response = await axios.get("/admin/loggins", {
      params: queryParams,
    });

    return response.data;
  }

  public static async DeclineTeacherAttempt(id: string): Promise<void> {
    await axios.post(`/admin/teacher-attempts/${id}/decline`);
  }

  public static async AcceptTeacherAttempt(id: string): Promise<void> {
    await axios.post(`/admin/teacher-attempts/${id}/accept`);
  }
}

export interface LogginHistoryModel {
  loginDate: string;
  isSucceeded: boolean;
  userEmail: string;
}

export interface TeacherAttemptsListModel {
  certificateUrl: string;
  isAccepted: boolean;
  fullName: string;
  email: string;
  login: string;
  dateCreated: string;
  publicId: string;
}

export interface UserListModel {
  fullName: string;
  login: string;
  email: string;
  role: string;
}

export interface ClassroomListModel {
  dateCreated: string;
  className: string;
  classroomKey: string;
  teacherName: string;
}

export interface LogginHistoryListModel {
  loginDate: string;
  isSucceeded: boolean;
  login: string;
}

export interface LogginsFilter {
  loginDateFrom?: Date;
  loginDateTo?: Date;
  isSucceeded?: boolean;
}

export interface UsersFilter {
  role?: string;
  login?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
}

export interface ClassroomsFilter {
  className?: string;
  classKey?: string;
  dateCreatedFrom?: Date;
  dateCreatedTo?: Date;
  teacherName?: string;
}

export interface PaginationResult<T> {
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;

  items: T[];
}

export interface PaginationEntry {
  pageNumber: number;
  pageSize: number;
}
