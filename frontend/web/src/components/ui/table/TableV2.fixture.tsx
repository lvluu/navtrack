/* eslint-disable react-hooks/rules-of-hooks */
import { useValue } from "react-cosmos/client";
import { TableV2, ITableColumn } from "./TableV2";

type User = {
  email: string;
  role: string;
};

const users: User[] = Array.from({ length: 1000 }, (_, i) => ({
  email: `user${i}@codeagency.com`,
  role: "Viewer"
}));

const columns: ITableColumn<User>[] = [
  {
    labelId: "generic.email",
    rowClassName: "py-1",
    value: (user) => user.email,
    render: (user) => user.email,
    sortable: true
  },
  {
    labelId: "generic.role",
    rowClassName: "py-1",
    value: (user) => user.role,
    render: (user) => user.role,
    sortable: true
  }
];

export default {
  Default: <TableV2 className="h-80" columns={columns} rows={users} />,
  "With footer": () => {
    const columnsWithFooter = columns.map((column, i) => ({
      ...column,
      footer: () => `Footer ${i + 1}`
    }));

    return (
      <TableV2 className="h-80" columns={columnsWithFooter} rows={users} />
    );
  },
  Loading: <TableV2 columns={columns} rows={undefined} />,
  "No Items": <TableV2 columns={columns} rows={[]} />,
  "With Selected Item": () => {
    const [user, setUser] = useValue<User | undefined>("user", {
      defaultValue: users[2]
    });

    return (
      <TableV2
        className="h-80"
        columns={columns}
        rows={users}
        setSelectedItem={setUser}
        selectedItem={user}
        equals={(a, b) => a.email === b.email}
      />
    );
  }
};