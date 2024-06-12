import { IController } from "../../../../domain/contracts/i-controller";
import { UrlUtility } from "../../../../utilities/url-utility";
import { GeneralSettingsForm } from "./general-settings-form";



export class GeneralCommunitySettingsPageController implements IController
{

    private readonly _communityName = UrlUtility.getCurrentPathValue(1);
    private readonly _form: GeneralSettingsForm;


    constructor()
    {
        this._form = new GeneralSettingsForm(this._communityName);
    }

    public control = () =>
    {
        this._form.control();
    }

}