import { VoteType } from "../enum/vote-type";


export type VoteScores = {
    votesCountUp?: number;
    votesCountDown?: number;
    votesCountNone?: number;
    votesScore?: number;
}


export type UserVoteSelection = {
    userVoteSelection?: VoteType;
}


export type UserVoteScores = VoteScores & UserVoteSelection;