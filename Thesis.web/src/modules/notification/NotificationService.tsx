import axios from "axios";
import type { PaginationEntry, PaginationResult } from "../admin/AdminService";

export default class NotificationService {
  public static async GetCurrentUserNotifications(): Promise<
    NotificationDetails[]
  > {
    const response = await axios.get("/notifications");
    return response.data;
  }

  public static async GetNotificationsFromParents(
    filter: NotificationListFilter,
    pagination: PaginationEntry,
    classroomId: string
  ): Promise<PaginationResult<NotificationDetails>> {
    const queryParams = {
      ...filter,
      ...pagination,
    };
    const response = await axios.get(
      `/notifications/${classroomId}/from-parents`,
      {
        params: queryParams,
      }
    );
    return response.data;
  }

  public static async GetNotificationsFromStudents(
    filter: NotificationListFilter,
    pagination: PaginationEntry,
    classroomId: string
  ): Promise<PaginationResult<NotificationDetails>> {
    const queryParams = {
      ...filter,
      ...pagination,
    };

    const response = await axios.get(
      `/notifications/${classroomId}/from-students`,
      {
        params: queryParams,
      }
    );
    return response.data;
  }

  public static async SendNotification(
    model: NotificationModel
  ): Promise<void> {
    await axios.post("/notifications/send", model);
  }

  public static async GetMessagesDictionary(): Promise<MessageType[]> {
    const response = await axios.get("/notifications/message-dictionary");
    return response.data;
  }

  public static async MarkAsSeen(id: string): Promise<void> {
    await axios.post(`/notifications/${id}/mark-as-seen`);
  }
}

export interface NotificationDetails {
  publicId: string;
  notifiedBy: string;
  isSeen: boolean;
  notificationType: string;
  message: string;
  notificationDate: string;
  isSystemNotification: boolean;
}

export interface MessageType {
  message: string;
  messageId: string;
}

export interface NotificationModel {
  userToId: string;
  messageId: string;
}

export interface NotificationListFilter {
  isSeen?: boolean;
  notifiedBy?: string;
  notificationDate?: Date;
}
