import { VoteType } from "../enum/vote-type";


export interface IVoted
{
    upvoted(): VoteType;
    downvoted(): VoteType;
}