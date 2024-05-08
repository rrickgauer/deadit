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
    postId: string;
}



