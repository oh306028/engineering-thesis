import axios from "axios";

export default class ClassroomService {
  public static async GetMineClassroom(): Promise<ClassroomDetails | null> {
    const response = await axios.get("/classroom/mine-classroom");
    return response.data;
  }

  public static async GetMyHomeWork(): Promise<HomeworkDetails[]> {
    const response = await axios.get("/classroom/mine-homework");
    return response.data;
  }

  public static async GetStudentsForClassroom(
    id: string
  ): Promise<StudentDetails[]> {
    const response = await axios.get(`/classroom/${id}/students`);
    return response.data;
  }

  public static async GetList(): Promise<ClassroomDetails[]> {
    const response = await axios.get(`/classroom`);
    return response.data;
  }

  public static async GetClassroomRequests(
    id: string
  ): Promise<StudentDetails[]> {
    const response = await axios.get(`/classroom/${id}/student-requests`);
    return response.data;
  }

  public static async AcceptStudent(
    classroomId: string,
    studentId: string
  ): Promise<void> {
    await axios.post(`/classroom/${classroomId}/student/${studentId}/accept`);
  }

  public static async DeclineStudent(
    classroomId: string,
    studentId: string
  ): Promise<void> {
    await axios.post(`/classroom/${classroomId}/student/${studentId}/decline`);
  }

  public static async JoinClassroom(model: JoinClassroomModel): Promise<void> {
    await axios.post("/classroom/join", model);
  }

  public static async GetStudentInfoForParent(): Promise<StudentDetailsWithClassroom> {
    const response = await axios.get(`/student/for-parent`);
    return response.data;
  }

  public static async GetHomeworkTypesDictionary(): Promise<
    HomeWorkTypePair[]
  > {
    const response = await axios.get(`/classroom/homework-types`);
    return response.data;
  }

  public static async CreateHomeWorkExercise(
    id: string,
    model: HomeWorkModel
  ): Promise<void> {
    await axios.post(`/classroom/${id}/homework`, model);
  }
}

export interface JoinClassroomModel {
  classroomKey: string;
}

export interface HomeWorkTypePair {
  key: string;
  value: string;
}

export interface ClassroomCreateModel {
  className: string;
}

export interface ClassroomDetails {
  className: string;
  classroomKey: string;
  teacherName: string;
  publicId: string;
  teacherPublicId: string;
}

export interface HomeWorkModel {
  subject: string;
  description: string;
  title: string;
  deadline: string | Date | null;
  exercises: ExerciseHomeWorkDetails[];
  type: string;
}

export interface StudentDetails {
  publicId: string;
  name: string;
  level: number;
  currentPoints: number;
  badgesCount: number;
  lastSeenAt: string;
  isCurrentUser: boolean;
}

export interface StudentDetailsWithClassroom extends StudentDetails {
  teacherPublicId: string;
  teacherName: string;
}

export interface HomeworkDetails {
  type: string;
  deadLine: string;
  dateCreated: string;
  exercises: ExerciseHomeWorkDetails[];
  title: string;
  description: string;
  subject: string;
  publicId: string;
}

export interface ExerciseHomeWorkDetails {
  content: string;
}

export interface ClassroomList {
  publicId: string;
  className: string;
}
