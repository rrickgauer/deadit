import { Guid } from "../domain/types/aliases";


export class GuidUtility
{
    public static getRandomGuid = (): Guid =>
    {
        return crypto.randomUUID();
    }
}