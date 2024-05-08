

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

