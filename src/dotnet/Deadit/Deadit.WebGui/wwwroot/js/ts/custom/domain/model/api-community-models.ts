

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


export class CommunityApiRequest
{
    public id?: number;
    public name?: string;
    public title?: string;
    public ownerId?: number;
    public description?: string;
    public createdOn?: string;
    public countMembers?: number;
    public urlGui?: string;
}