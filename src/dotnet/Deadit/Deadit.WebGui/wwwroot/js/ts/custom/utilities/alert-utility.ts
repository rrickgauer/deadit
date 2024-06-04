import { AlertBase, AlertDanger, AlertParms, AlertSuccess } from "../domain/helpers/alerts/page-alerts";
import { ErrorMessage } from "../domain/model/api-response";
import { ErrorMessageTemplate } from "../templates/error-message-template";


export interface AlertConstructor<T extends AlertBase>
{
    new(args: AlertParms): T;
}

export class AlertUtility
{

    public static showSuccess = (args: AlertParms) => AlertUtility.showAlert(args, AlertSuccess);
    public static showDanger = (args: AlertParms) => AlertUtility.showAlert(args, AlertDanger);

    private static showAlert = <TAlert extends AlertBase>(args: AlertParms, alertConstructor: AlertConstructor<TAlert>): TAlert =>
    {
        const pageAlert = new alertConstructor(args);
        pageAlert.show();
        return pageAlert;
    }

    public static showErrors(args: { container: HTMLDivElement, errors: ErrorMessage[] }): AlertDanger
    {
        const templateEngine = new ErrorMessageTemplate();
        const html = templateEngine.toHtmls(args.errors);

        const a = new AlertDanger({
            container: args.container,
            message: html,
        });

        a.show();

        return a;
    }
}