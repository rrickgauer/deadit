
import { Modal } from "bootstrap";

export class BootstrapUtility
{

    public static getModal(element: Element): Modal
    {
        return Modal.getOrCreateInstance(element);
    }

    public static showModal(element: Element)
    {
        Modal.getOrCreateInstance(element).show();
    }

}