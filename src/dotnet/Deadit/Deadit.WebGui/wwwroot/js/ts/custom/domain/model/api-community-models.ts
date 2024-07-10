import { CommunityType } from "../enum/community-type";
import { FlairPostRule } from "../enum/flair-post-rule";
import { TextPostBodyRule } from "../enum/text-post-body-rule";


export class CreateCommunityApiRequest
{
    public name: string;
    public title: string;
    public description: string;

    constructor(name: string, title: string, description: string)
    {
        this.name = name;
        this.title = title;
        this.description = description;
    }
}


export type CommunityApiRequest = {
    communityId?: number;
    communityName?: string;
    communityTitle?: string;
    communityOwnerId?: number;
    communityDescription?: string;
    communityCreatedOn?: string;
    communityCountMembers?: number;
    communityUrlGui?: string;
}



export type UpdateCommunityApiRequest = {
    title: string;
    description: string | null;
    communityType: CommunityType;
    textPostBodyRule: TextPostBodyRule;
    acceptingNewMembers: boolean;
    flairPostRule: FlairPostRule;
}

export class UpdateCommunityRequest
{
    public readonly communityName: string;
    public readonly data: UpdateCommunityApiRequest;

    constructor(communityName: string, data: UpdateCommunityApiRequest)
    {
        this.communityName = communityName;
        this.data = data;
    }
}




