import { VoteScore } from "../domain/helpers/vote-scores/vote-score";
import { UserVoteScores, VoteScores } from "../domain/model/common-api-response-types";
import { HtmlString } from "../domain/types/aliases";
import { HtmlTemplate } from "./html-template";



export class VoteScoreTemplate extends HtmlTemplate<UserVoteScores>
{
    public toHtml(model: UserVoteScores): HtmlString
    {
        let icons = VoteScore.getVoteIcons(model);

        let html = `

            <div class="item-voting" data-current-vote="${model.userVoteSelection}">
                <button class="btn btn-sm btn-reset btn-vote btn-vote-up" title="Upvote" type="button">${icons.Upvote}</button>
                <div class="item-voting-score">${model.votesScore ?? 0}</div>
                <button class="btn btn-sm btn-reset btn-vote btn-vote-down" title="Downvote" type="button">${icons.Downvote}</button>
            </div>
        `;
        
        return html;
    }
}