import { Form, Formik, FormikHelpers } from "formik";
import { useCallback, useMemo } from "react";
import useCurrentUser from "../../hooks/app/useCurrentUser";
import { nameOf } from "../../utils/typescript";
import Button from "../ui/shared/button/Button";
import FormikTextInput from "../ui/shared/text-input/FormikTextInput";
import SettingsLayout from "./SettingsLayout";
import { FormattedMessage, useIntl } from "react-intl";
import FormikSelect from "../ui/shared/select/FormikSelect";
import { UnitsType } from "../../api/model/custom/UnitsType";
import { ISelectOption } from "../ui/shared/select/types";
import { useUpdateUserMutation } from "../../hooks/mutations/useUpdateUserMutation";
import useNotification from "../ui/shared/notification/useNotification";
import { mapErrors } from "../../utils/formik";

type AccountSettingsFormValues = {
  email?: string;
  units?: string;
};

export default function SettingsAccountPage() {
  const user = useCurrentUser();
  const updateUserMutation = useUpdateUserMutation();
  const { showNotification } = useNotification();
  const intl = useIntl();

  const handleSubmit = useCallback(
    (
      values: AccountSettingsFormValues,
      formikHelpers: FormikHelpers<AccountSettingsFormValues>
    ) => {
      updateUserMutation.mutate(
        {
          data: {
            email: values.email,
            unitsType: values.units === UnitsType.Metric ? 0 : 1
          }
        },
        {
          onSuccess: () => {
            showNotification({
              type: "success",
              description: intl.formatMessage({
                id: "settings.account.success"
              })
            });
            formikHelpers.resetForm();
          },
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [intl, showNotification, updateUserMutation]
  );

  const units: ISelectOption[] = useMemo(
    () => [
      {
        label: intl.formatMessage({ id: "generic.units.metric" }),
        value: UnitsType.Metric
      },
      {
        label: intl.formatMessage({ id: "generic.units.imperial" }),
        value: UnitsType.Imperial
      }
    ],
    [intl]
  );

  return (
    <SettingsLayout>
      {user && (
        <Formik<AccountSettingsFormValues>
          initialValues={{
            email: user?.email,
            units: user?.units
          }}
          onSubmit={(values, formikHelpers) =>
            handleSubmit(values, formikHelpers)
          }>
          {() => (
            <Form>
              <div className="shadow sm:rounded-md sm:overflow-hidden">
                <div className="bg-white py-6 px-4 space-y-6 sm:p-6">
                  <div>
                    <h3 className="text-lg leading-6 font-medium text-gray-900">
                      <FormattedMessage id="settings.account.title" />
                    </h3>
                  </div>
                  <div className="grid grid-cols-6 gap-6">
                    <div className="col-span-3">
                      <FormikTextInput
                        name={nameOf<AccountSettingsFormValues>("email")}
                        label="generic.email"
                      />
                    </div>
                    <div className="col-span-3"></div>
                    <div className="col-span-3">
                      <FormikSelect
                        name={nameOf<AccountSettingsFormValues>("units")}
                        label="generic.units"
                        options={units}
                      />
                    </div>
                  </div>
                </div>
                <div className="px-3 py-3 bg-gray-50 text-right">
                  <Button size="lg" type="submit">
                    <FormattedMessage id="generic.save" />
                  </Button>
                </div>
              </div>
            </Form>
          )}
        </Formik>
      )}
    </SettingsLayout>
  );
}