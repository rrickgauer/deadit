
import { Modal } from "bootstrap";

export class BootstrapUtility
{
    public static showModal(element: Element)
    {
        Modal.getOrCreateInstance(element).show();
    }

}