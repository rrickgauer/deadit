import { DateTimeString } from "../types/aliases";
import { CommunityApiRequest } from "./api-community-models";

export type GetFlairPostApiResponse = CommunityApiRequest & {
    flairPostId?: number;
    flairPostCommunityId?: number;
    flairPostName?: string;
    flairPostColor?: string;
    flairPostCreatedOn?: DateTimeString;
}


export type SaveFlairPostData = {
    name: string,
    color: string,
}


export type FlairPostApiRequestBody = SaveFlairPostData & {
    communityName: string;
}





