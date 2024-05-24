import { PostType } from "../enum/post-type";
import { DateTimeString, Guid } from "../types/aliases";
import { CommunityApiRequest } from "./api-community-models";



export type PostApiRequest = {
    title: string;
}

export type CreateTextPostApiRequest = PostApiRequest & {
    content?: string;
}

export type CreateLinkPostApiRequest = PostApiRequest & {
    url: string;
}


export type UpdateTextPostApiRequest = {
    content: string;
}


export type PostApiResponse = CommunityApiRequest & {
    postType?: PostType;
    postId?: Guid;
    postCommunityId?: number;
    postTitle?: string;
    postAuthorId?: number;
    postCreatedOn?: DateTimeString;
    postDeletedOn?: DateTimeString;
    postArchivedOn?: DateTimeString;
    postModRemovedOn?: DateTimeString;
    postUriWeb?: string;
    postUriApi?: string;
    postCountComments?: number;
}

export type TextPostApiResponse = PostApiResponse & {
    postContent?: string;
}

export type LinkPostApiResponse = PostApiResponse & {
    postUrl?: string;
}



export type PostPageParms = {
    communityName: string;
    postId: Guid;
}





