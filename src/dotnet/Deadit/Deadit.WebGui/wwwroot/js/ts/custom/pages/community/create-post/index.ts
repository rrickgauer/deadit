import { NativeEvents } from "../../../domain/constants/native-events";
import { MessageBoxConfirm } from "../../../domain/helpers/message-box/MessageBoxConfirm";
import { MessageBoxUtilities } from "../../../utilities/message-box-utilities";
import { PageUtilities } from "../../../utilities/page-utilities";
import { CreatePostController } from "./create-post-controller";

/**
 * Main logic
 */
PageUtilities.pageReady(async () =>
{
    const controller = new CreatePostController();
    controller.control();


    testMessageBox();

});



function testMessageBox()
{

    document.querySelector('#btn-standard').addEventListener(NativeEvents.Click, (e) =>
    {
        MessageBoxUtilities.showStandard({
            message: 'Shit',
            title: 'standard',
        });
    });


    document.querySelector('#btn-error').addEventListener(NativeEvents.Click, (e) =>
    {
        MessageBoxUtilities.showError({
            message: 'bad one',
            title: 'error',
        });
    });


    document.querySelector('#btn-success').addEventListener(NativeEvents.Click, (e) =>
    {
        MessageBoxUtilities.showSuccess({
            message: 'good job',
            title: 'fuck u nice',
        });
    });


    document.querySelector('#btn-confirm').addEventListener(NativeEvents.Click, (e) =>
    {
        const confirmMessage = new MessageBoxConfirm('Are you sure?');

        confirmMessage.confirm({
            onSuccess: () =>
            {
                alert('yes');
            },
        });

    });


}

