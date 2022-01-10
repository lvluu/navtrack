import { useCallback } from "react";
import { useHistory } from "react-router";
import { useGetTokenMutation } from "../mutations/useGetTokenMutation";
import { LoginFormValues } from "../../components/login/LoginFormValues";
import { AUTHENTICATION } from "../../constants";
import { useSetRecoilState } from "recoil";
import { authenticationAtom } from "../../state/app.authentication";
import { add } from "date-fns";

export const useLogin = () => {
  const setState = useSetRecoilState(authenticationAtom);
  const history = useHistory();

  const getTokenMutation = useGetTokenMutation({
    onSuccess: (data) => {
      setState((current) => ({
        ...current,
        isAuthenticated: true,
        token: {
          accessToken: data.access_token,
          refreshToken: data.refresh_token,
          expiryDate: add(new Date(), {
            seconds: data.expires_in
          }).toISOString()
        }
      }));

      history.push("/");
    }
  });

  const login = useCallback(
    (values: LoginFormValues) => {
      const data = {
        grant_type: "password",
        username: values.email,
        password: values.password,
        scope: "offline_access IdentityServerApi openid",
        client_id: AUTHENTICATION.CLIENT_ID
      };

      getTokenMutation.mutate(data);
    },
    [getTokenMutation]
  );

  return { login, loading: getTokenMutation.isLoading };
};