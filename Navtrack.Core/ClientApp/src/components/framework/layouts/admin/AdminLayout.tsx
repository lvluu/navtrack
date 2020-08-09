import React from "react";
import AdminSidebar from "./AdminSidebar";
import AdminNavbar from "./AdminNavbar";
import AdminFooter from "./AdminFooter";
import classNames from "classnames";
import Error from "components/library/error/Error";
import { ApiError } from "framework/httpClient/AppError";

type Props = {
  children: React.ReactNode;
  hidePadding?: boolean;
  error?: ApiError<object>;
};

export default function AdminLayout(props: Props) {
  return (
    <div className="flex min-h-screen flex-col bg-gray-100" style={{ minWidth: "800px" }}>
      <AdminNavbar />
      <div className="flex flex-row flex-grow">
        <AdminSidebar />
        <div className={classNames("flex flex-grow flex-col", { "p-5 pb-2": !props.hidePadding })}>
          <Error error={props.error} />
          {props.children}
          <AdminFooter className={classNames({ "m-5": props.hidePadding })} />
        </div>
      </div>
    </div>
  );
}
