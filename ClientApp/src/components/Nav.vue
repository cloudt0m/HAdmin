<template>
	<div>
		<v-app-bar
			color="cyan accent-4"
			dense
			dark
		>

			<v-toolbar-title>登入介面</v-toolbar-title>

			<v-spacer></v-spacer>

			<v-menu
				left
				bottom
				v-if="isLoggedIn"
			>
				<template v-slot:activator="{ on, attrs }">
					<v-btn
						icon
						v-bind="attrs"
						v-on="on"
					>
						<v-icon>mdi-dots-vertical</v-icon>
					</v-btn>
				</template>

				<v-list>
					<v-list-item @click="logout()">
						<v-list-item-title>登出</v-list-item-title>
					</v-list-item>
				</v-list>
			</v-menu>
		</v-app-bar>
	</div>
</template>

<script>
import service from "../utils/request";

export default {
	name: "Nav",
	props: ["isLoggedIn"],
	data() {
		return {};
	},
	methods: {
		logout: function () {
			service
				.get("/auth/logout")
				.then(() => this.$router.push("/"))
				.catch((err) => console.log(err));
		},
	},
};
</script>
