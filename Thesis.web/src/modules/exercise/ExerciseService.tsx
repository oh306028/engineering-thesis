import axios from "axios";

export default class ExerciseService {
  public static async GetExercise(id: string): Promise<ExerciseDetails> {
    const response = await axios.get(`/exercise/${id}`);
    return response.data;
  }

  public static async Answer(id: string, answer: AnswerModel): Promise<void> {
    await axios.post(`/exercise/${id}/answer`, answer);
  }

  public static async GameAnswer(
    id: string,
    sessionId: string,
    answer: AnswerModel
  ): Promise<void> {
    await axios.post(`/exercise/${id}/game/${sessionId}/answer`, answer);
  }
}

export interface AnswerDetails {
  correctText: string;
  correctNumber?: number;
  correctOption: string;
  incorrectOption1: string;
  incorrectOption2: string;
  incorrectOption3: string;
}

export interface ExerciseDetails {
  publicId: string;
  level: string;
  isDone: boolean;
  subject: string;
  answer: AnswerDetails;
  learningPath: string;
  content: string;
}

export interface AnswerModel {
  correctText: string;
  correctNumber?: number | null;
  correctOption: string;
}
