import { AlertBase, AlertDanger, AlertParms, AlertSuccess } from "../domain/helpers/alerts/page-alerts";


export interface AlertConstructor<T extends AlertBase>
{
    new(args: AlertParms): T;
}

export class AlertUtilities
{

    public static showSuccess = (args: AlertParms) => AlertUtilities.showAlert(args, AlertSuccess);
    public static showDanger = (args: AlertParms) => AlertUtilities.showAlert(args, AlertDanger);


    private static showAlert = <TAlert extends AlertBase>(args: AlertParms, alertConstructor: AlertConstructor<TAlert>): TAlert =>
    {
        const pageAlert = new alertConstructor(args);
        pageAlert.show();
        return pageAlert;
    }
}