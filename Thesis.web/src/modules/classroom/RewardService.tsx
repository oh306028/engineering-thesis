import axios from "axios";

export default class RewardService {
  public static async GetBadgeByLearningPath(
    learningPathId: string
  ): Promise<BadgeDetails> {
    const response = await axios.get(`/rewards/${learningPathId}`, {});
    return response.data;
  }

  public static async AreAnyNewRewards(): Promise<boolean> {
    const response = await axios.get(`/rewards/new-rewards`);
    return response.data;
  }

  public static async MarkAsSeen(): Promise<void> {
    const response = await axios.post(`/rewards/mark-as-seen`);
    return response.data;
  }

  public static async GetMineBadges(): Promise<BadgeDetails[]> {
    const response = await axios.get(`/rewards/mine-badges`);
    return response.data;
  }

  public static async GetMineAchievements(): Promise<AchievementDetails[]> {
    const response = await axios.get(`/rewards/mine-achievements`);
    return response.data;
  }

  public static async GetAchievements(): Promise<AchievementDetails[]> {
    const response = await axios.get(`/rewards/achievements`);
    return response.data;
  }
}

export interface BadgeDetails {
  publicId: string;
  name: string;
  emote: string;
}

export interface AchievementDetails {
  name: string;
  description: string;
  badge: BadgeDetails;
}
