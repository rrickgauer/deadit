import { DateTimeString } from "../types/aliases";
import { CommunityApiRequest } from "./api-community-models";



export type FlairPostOnlyApiResponse = {
    flairPostId?: number;
    flairPostCommunityId?: number;
    flairPostName?: string;
    flairPostColor?: string;
    flairPostCreatedOn?: DateTimeString;
}


export type GetFlairPostApiResponse = CommunityApiRequest & FlairPostOnlyApiResponse;


export type SaveFlairPostData = {
    name: string,
    color: string,
}


export type FlairPostApiRequestBody = SaveFlairPostData & {
    communityName: string;
}





