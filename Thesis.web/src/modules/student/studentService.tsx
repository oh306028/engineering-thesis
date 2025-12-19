import axios from "axios";
import type { BadgeDetails } from "../classroom/RewardService";

export default class StudentService {
  public static async GetPaths(type: string): Promise<LearningPathDetails[]> {
    const response = await axios.get("/learning-path", {
      params: {
        type: type,
      },
    });

    return response.data;
  }

  public static async GetDrafts(): Promise<LearningPathDetails[]> {
    const response = await axios.get("/learning-path/drafts");

    return response.data;
  }

  public static async RemoveDraft(id: string): Promise<void> {
    await axios.delete(`/learning-path/${id}`);
  }

  public static async Publish(id: string): Promise<void> {
    await axios.patch(`/learning-path/${id}`);
  }

  public static async CreatePath(model: LearningPathModel): Promise<void> {
    await axios.post(`/learning-path`, model);
  }

  public static async CreatePathExercise(
    id: string,
    model: ExercisePathModel
  ): Promise<void> {
    await axios.post(`/learning-path/${id}/exercise`, model);
  }

  public static async SetStudentLearningFilters(
    model: StudentFilterModel
  ): Promise<void> {
    await axios.put("/student/learning", model);
  }

  public static async GetStudentProgress(
    currentLevel: number
  ): Promise<StudentProgressDetails> {
    const response = await axios.get("/student/progress", {
      params: {
        level: currentLevel,
      },
    });
    return response.data;
  }

  public static async GetPathExercise(
    id: string,
    isReviewPath?: boolean
  ): Promise<PathExercisesResource> {
    const response = await axios.get(`/learning-path/${id}/exercise`, {
      params: {
        isReviewPath: isReviewPath,
      },
    });

    return response.data;
  }
}
export interface StudentProgressDetails {
  level: number;
  currentPoints: number;
  minLevelPoints: number;
  maxLevelPoints: number;
  newLevel: boolean;
}

export interface LearningPathModel {
  level: number;
  name: string;
  subjectId: string;
  badgeId: string;
}

export interface LearningPathDetails {
  name: string;
  type: string;
  level: number;
  publicId: string;
  subject?: string;
  badges: BadgeDetails[];
  isCurrentPathFinished: boolean;
}

export interface LearningPathDetails {
  name: string;
  type: string;
  level: number;
  publicId: string;
}

export interface PathExercise {
  publicId: string;
  isCompleted: boolean;
  title?: string;
  description?: string;
}

export interface PathExercisesResource {
  count: number;
  exercises: PathExercise[];
}

export interface StudentFilterModel {
  level: number;
  subjectId: string;
}

export interface AnswerExerciseModel {
  incorrectOption1?: string;
  incorrectOption2?: string;
  incorrectOption3?: string;
  correctText?: string;
  correctNumber?: number;
  correctOption: string;
}

export interface ExercisePathModel {
  content: string;
  answer: AnswerExerciseModel;
}
