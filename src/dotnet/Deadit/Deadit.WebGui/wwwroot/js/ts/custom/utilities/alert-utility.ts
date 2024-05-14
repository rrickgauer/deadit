import { AlertBase, AlertDanger, AlertParms, AlertSuccess } from "../domain/helpers/alerts/page-alerts";


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
}