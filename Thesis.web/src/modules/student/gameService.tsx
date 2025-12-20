import axios from "axios";
import type { ExerciseDetails } from "../exercise/ExerciseService";

export default class GameService {
  public static async StartSession(): Promise<string> {
    const response = await axios.post("/games/start");

    return response.data;
  }

  public static async GetQuestion(id: string): Promise<ExerciseDetails> {
    const response = await axios.get(`/games/${id}`);

    return response.data;
  }

  public static async GetGameDetails(id: string): Promise<GameDetails> {
    const response = await axios.get(`/games/${id}/details`);

    return response.data;
  }
}

export interface GameDetails {
  questionsCount: number;
  correctAnswers: number;
  wrongAnswers: number;
}
