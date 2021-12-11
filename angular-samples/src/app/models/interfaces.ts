import { eNotificationType } from "./enums";

export interface INotification {
    message: string,
    type: eNotificationType,
    duration: number
}