import axios from "axios";

export default class DictionaryService {
  public static async GetSubjects(): Promise<KeyValuePair[]> {
    const response = await axios.get("dictionary/subjects");
    return response.data;
  }

  public static async GetBadges(): Promise<KeyValuePair[]> {
    const response = await axios.get("dictionary/badges");
    return response.data;
  }
}

export interface KeyValuePair {
  key: string;
  value: string;
}
