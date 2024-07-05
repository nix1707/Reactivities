import { observer } from "mobx-react-lite"
import { useStore } from "../../app/stores/store"
import { Form, Formik } from "formik";
import * as Yup from 'yup'
import MyTextInput from "../../app/common/form/MyTextInput";
import MyTextArea from "../../app/common/form/MyTextArea";
import { Button } from "semantic-ui-react";

interface Props {
    setEditMode: (mode: boolean) => void;
}

const ProfileEditForm = ({ setEditMode }: Props) => {
    const { profileStore: { updateProfile, profile } } = useStore();

    return (
        <Formik initialValues={{ displayName: profile?.displayName, bio: profile?.bio }}
            onSubmit={values => {
                updateProfile(values).then(() => setEditMode(false));
            }}
            validationSchema={Yup.object({
                displayName: Yup.string().required()
            })}
        >
            {({isSubmitting, isValid, dirty}) => (
                <Form className='ui form'>
                    <MyTextInput placeholder={"Display Name"} name={"displayName"} />
                    <MyTextArea placeholder={"Add your bio"} name={"bio"} rows={3} />

                    <Button 
                        positive
                        type="submit"
                        loading={isSubmitting}
                        content="Update profile"
                        floated="right"
                        disabled={!isValid || !dirty}
                    />
                </Form>
            )}
        </Formik>
    )
}

export default observer(ProfileEditForm)