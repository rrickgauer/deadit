import { NativeEvents } from "../../../domain/constants/native-events";
import { IController } from "../../../domain/contracts/i-controller";
import { CommentElement } from "./comment";
import { CommentForm } from "./comment-form";


export class AuthoredCommentElement extends CommentElement implements IController
{
    public btnEdit: HTMLAnchorElement;
    public btnDelete: HTMLAnchorElement;
    public editForm: CommentForm;

    //public editForm: HTMLFormElement;
    //public btnCancelEdit: HTMLButtonElement;
    //public editContentInput: HTMLTextAreaElement;
    //public btnEditFormSubmit: HTMLButtonElement;

    

    private _originalContent: string;
    

    constructor(element: Element)
    {
        super(element);

        this.btnEdit = this.element.querySelector('.btn-comment-action-edit') as HTMLAnchorElement;
        this.btnDelete = this.element.querySelector('.btn-comment-action-delete') as HTMLAnchorElement;
        this.editForm = new CommentForm(this.element.querySelector('.form-post-comment'));
    }


    public control()
    {
        super.control();
        this.addListeners();
    }


    private addListenersSuper = this.addListeners;

    protected addListeners = () =>
    {
        this.addListenersSuper();
        
        this.btnEdit.addEventListener(NativeEvents.Click, (e) =>
        {
            e.preventDefault();
            this.onEditBtnClick();
        });

        this.btnDelete.addEventListener(NativeEvents.Click, (e) =>
        {
            e.preventDefault();
            this.onDeleteBtnClick();
        });

        this.btnToggle.addEventListener(NativeEvents.Click, (e) =>
        {
            e.preventDefault();
            this.onToggleBtnClick();
        });
    }


    private onEditBtnClick = () =>
    {
        this.showForm();
    }

    private onDeleteBtnClick = () =>
    {
        alert('delete');
    }


    private onCancelEditBtnClick = () =>
    {
        this.showDisplay();
        //this.editContentInput.value = this._originalContent;
    }

    private onEditFormSubmit = () =>
    {
        alert('submit');
    }

    



    private showForm = () => this.element.classList.add('editing');
    private showDisplay = () => this.element.classList.remove('editing');


}