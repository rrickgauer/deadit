import { PostType } from "../enum/post-type";
import { CommunityApiRequest } from "./api-community-models";



export type PostApiRequest = {
    title: string;
}

export type TextPostApiRequest = PostApiRequest & {
    content?: string;
}

export type LinkPostApiRequest = PostApiRequest & {
    url: string;
}




export type PostApiResponse = CommunityApiRequest & {
    postType?: PostType;
    postId?: string;
    postCommunityId?: number;
    postTitle?: string;
    postAuthorId?: number;
    postCreatedOn?: string;
    uriWeb?: string;
    uriApi?: string;
}

export type TextPostApiResponse = PostApiResponse & {
    postContent?: string;
}

export type LinkPostApiResponse = PostApiResponse & {
    postUrl?: string;
}





