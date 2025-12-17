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

export interface LearningPathDetails {
  name: string;
  type: string;
  level: number;
  publicId: string;
  badges: BadgeDetails[];
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
